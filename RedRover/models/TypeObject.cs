public class TypeObject{
    public String? id{get;set;}
    public String? name{get;set;}
    public CustomField[]? customFields{get;set;}

    public TypeObject(){

    }

    public TypeObject(string idInput, string nameInput, string[] customFieldsInput){
        this.id = idInput;
        this.name = nameInput;
        foreach(string str in customFieldsInput){
            this.customFields.Append(new CustomField(str));
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
                else if(prop.PropertyType == typeof(CustomField)){
                    bool empty = ((CustomField)val).isEmpty();
                    if(!empty) return empty;
                }
            }
        }
        return true;
    }
}