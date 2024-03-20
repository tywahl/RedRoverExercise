using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Reflection;

public class PrintController{
    public PrintController(){

    }
   // ideally this would be in schema file
    private string schemaA = @"{
        'Order':[
            'id',
            'name',
            'email',
            {'type':
                [
                'id',
                'name',
                {'customFields':[]}
                ]
            },
            'externalId'
        ]
    }";

   // ideally these would be in schema files
    private string schemaB = @"{
        'Order':[
            'email',
            'externalId',
            'id',
            'name',
            {
                'type':
                [
                    {'customFields':[]  },
                    'id',
                    'name',
              
                ]
            }
        ]
        }";
    public void printAll(BaseObject bo){
        if(bo.isEmpty()){ Console.WriteLine("Invalid Object Input " );return;}
        Console.WriteLine("Output style 1");
        print(bo, schemaA);
        Console.WriteLine("\n\n Output style 2 ");
        print(bo, schemaB);
    }
    public void print(BaseObject bo, string schema){
        if(schema == null){
            schema = schemaA;
        }
        dynamic dynamicObject = JObject.Parse(schema);
        printWithSchema(bo, dynamicObject.Order, 0);
    }

    private void printWithSchema(Object bo, dynamic schema, int tabIndex){
        if(schema.GetType() == typeof(Newtonsoft.Json.Linq.JObject)){
           
            foreach(var property in schema){
                if(property.Value.GetType() == typeof(Newtonsoft.Json.Linq.JObject) || property.Value.GetType() == typeof(Newtonsoft.Json.Linq.JArray)){
                   printValue(property.Name, tabIndex);
                }
                 tabIndex++;
                if(property.Name=="type") {
                    PropertyInfo prop =  bo.GetType().GetProperty(property.Name.ToString());
                    printWithSchema(prop.GetValue(bo), property.Value, tabIndex);
                }
                else{
                  PropertyInfo prop =  bo.GetType().GetProperty(property.Name.ToString());
                  printWithSchema(prop.GetValue(bo, null), property.Value, tabIndex);
                }
            }
        }
        else if(schema.GetType() == typeof(Newtonsoft.Json.Linq.JArray)){
            if(schema.Count==0){
                foreach(var item in (CustomField[])bo){
                    printValue(item.name, tabIndex);
                }
            }
            else{
                foreach(var item in schema){
                    if(item.GetType() == typeof(Newtonsoft.Json.Linq.JObject) || item.GetType() == typeof(Newtonsoft.Json.Linq.JArray)){
                        printWithSchema(bo, item, tabIndex);
                    }else{
                        PropertyInfo prop =  bo.GetType().GetProperty(item.ToString());
                        printWithSchema(prop.GetValue(bo), item, tabIndex); 
                    }
                }
            }
        }
        else if(schema.GetType()==typeof(Newtonsoft.Json.Linq.JValue)){
            printValue(bo, tabIndex);
        }
    }

    private void printValue(object bo, int tabIndex){
        if(bo == null) return;
        StringBuilder sbr = new StringBuilder();
        int i =0; 
        while(i<tabIndex){
            sbr.Append("  ");
            i++;
        }
        sbr.Append("- " + bo);
        Console.WriteLine(sbr.ToString());
    }
}