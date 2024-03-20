


public class CustomField{
    public String name;

    public CustomField(){
        name = null;
    }

    public CustomField(String inputName){
        this.name = inputName;
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
            }
        }
        return true;
    }
}