using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor.Serialization;

public record SolidMember
{

    public bool IsValid = true;
    public SolidType ParentType;
    public string MemberName;

    public SolidMember(SolidType __type, string __variableName) {
        if (__type == null) {
            Debug.Error("You cannot create a SolidMember with a null parent type!");
            IsValid = false;
            return;
        }

        if (__variableName == null) {
            Debug.Error("You cannot create a SolidMember with a null variable name!");
            IsValid = false;
            return;
        }

        MemberName = __variableName;
        ParentType = __type;
        
        IsValid = IsValid && ParentType.IsValid;
    }

    public SolidMember(MemberInfo __memberInfo) : this(new SolidType(__memberInfo.DeclaringType), __memberInfo.Name) { }

    public static SolidMember FromMemberName(string __parentTypeName, string __memberName) => new SolidMember(SolidType.FromTypeName(__parentTypeName), __memberName);

    public MemberInfo? FindMember() {
        Type? type = ParentType.FindType();

        if (type == null) {
            Debug.Error("Parent type failed to be found!");
            return null;
        }

        MemberInfo? member = type.GetMember(MemberName).FirstOrDefault();

        if (member == null)
        {
            Debug.Error("A member by that name is not defined in the parent type!");
            return null;
        }
        
        return member;
    }
}