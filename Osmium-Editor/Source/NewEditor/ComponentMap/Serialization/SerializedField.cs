using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor.Serialization;

/// <summary> Represents a Serialized variable which should be treated as the default value for corresponding Components, and stored in the Component Map
/// when the editor closes. </summary>
public class SerializedVariable
{



    public readonly SolidMember member;
    
    
    /// <summary> The value of the serialized field </summary>
    public readonly object? value;

    public readonly bool IsValid = true;
    
    

    public SerializedVariable(SolidMember __member, object? __value) {
        if (__member == null) {
            Debug.Error("The member cannot be null");
            IsValid = false;
            return;
        }
        
        MemberInfo? info = __member.FindMember();

        if (info == null) {
            Debug.Error("The variable name does not reference a real member!");
            IsValid = false;
            return;
        } 

        if (info.MemberType != MemberTypes.Field && info.MemberType != MemberTypes.Property) {
            Debug.Error("You cannot create a SerializedVariable with anything other than a Field or Property!");
            IsValid = false;
        }

        IsValid = IsValid && __member.IsValid;

        member = __member;
        
        value = __value;
    }



    public string ToSaveData() {
        return member.MemberName + "|" + value;
    }

    public static SerializedVariable? FromSaveData(Type __memberType, string __saveData) {
        string[] SaveParameters = __saveData.Split('|');

        if (SaveParameters.Length != 2) {
            Debug.Error("Invalid Save Data!");
            return null;
        }
        
        MemberInfo? foundMember = __memberType.GetMember(SaveParameters[0],
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty).FirstOrDefault();

        if (foundMember == null) {
            Debug.Error("No such member exists!", ["Member Name"], [SaveParameters[0]]);
            return null;
        }

        Type type = null;

        if (foundMember is FieldInfo fieldInfo) type = fieldInfo.FieldType;
        if (foundMember is PropertyInfo propertyInfo) type = propertyInfo.PropertyType;
        
        return new SerializedVariable(new SolidMember(new SolidType(__memberType), SaveParameters[0]), Convert.ChangeType(SaveParameters[1], type));
    } 

    public void Load(ComponentDocker newDocker) {
        //todo: GROSS
        MemberInfo? info = member.FindMember();
        
        if(info is FieldInfo fieldInfo) fieldInfo.SetValue(newDocker, value);
        if(info is PropertyInfo propertyInfo) propertyInfo.SetValue(newDocker, value, null);
    }

    public static bool SameTarget(SerializedVariable a, SerializedVariable b) {
        return a.member == b.member;
    }
}