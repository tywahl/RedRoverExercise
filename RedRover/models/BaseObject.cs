using System.Globalization;
using System.Linq;
using System.Reflection;
public class BaseObject{
    public string? id{get;set;}
    public string? name{get;set;}
    public string? email{get;set;}
    public  TypeObject type{get;set;}
    
    public string? externalId{get;set;}

    public BaseObject(){

    }

    public BaseObject(String info){
        List<String>[] parsedIntoArray = parseFromString(info);
        arraysToObject(parsedIntoArray);
    }

    

    public List<String>[] parseFromString(String info){
        var res = info.Replace(" ", "")
            .Split('(')
            .Select(sub=> sub.Split(','))
            .ToArray();
        //Initialize Array of Lists of strings
        List<String>[] arr = Enumerable.Range(0, 3).Select(_ => new List<String>()).ToArray();
        int objectIndex = 0;
        string currentProperty = "";
        foreach( char l in info.Replace(" ", "").ToCharArray()){
            if(l == ',' ||l == ')' ||l =='(') {
                if(currentProperty!=""  ){
                    arr[objectIndex].Add(currentProperty);
                }
                if(l == ')'){
                    objectIndex--;
                }
                if(l == '(' && arr[objectIndex].Count()>0){
                    objectIndex++;
                }
                currentProperty = "";
            }
            else currentProperty += l;
            
        }
        return arr;
    }

    private void arraysToObject(List<String>[] arrayInfo){
        ListToObject(arrayInfo[0], this);
        this.type = new TypeObject();
        ListToObject(arrayInfo[1],this.type);
        List<CustomField> customFields = new List<CustomField>();
        arrayInfo[2].ForEach( str=>{
           customFields.Add(new CustomField(str));
        });
        this.type.customFields = customFields.ToArray();
    }

    private void ListToObject(List<string> properties, Object obj){
        
        foreach(PropertyInfo prop in obj.GetType().GetProperties()){
            if(prop.PropertyType!=typeof(TypeObject) && !prop.PropertyType.IsArray){
                var result = properties.Find(x=>x.Equals(prop.Name.ToString()));
                prop.SetValue(obj, result );
            }
        }
    }

    public bool isEmpty(){
        var properties = this.GetType().GetProperties();
        foreach(var prop in properties){
            var val = prop.GetValue(this,null);
            if(val!=null)
            {
                if(prop.PropertyType == typeof(string)){
                    if((string)val!=""){
                        return false;
                    }
                }
                else if(prop.PropertyType == typeof(TypeObject)){
                    bool empty = ((TypeObject)val).isEmpty();
                    if(!empty) return empty;
                }
            }
        }
        return true;
    }
}