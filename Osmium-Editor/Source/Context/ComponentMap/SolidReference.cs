using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using OsmiumNucleus;
using Context = OsmiumEditor.Context;

namespace OsmiumEditor.ComponentMap;

public static partial class ComponentMap
{ 
    private class SolidReference
    {
        public string AssemblyName { get; set; }
        public string ComponentTypeName { get; set; }
        public string SceneName { get; set; }
        public string ComponentName { get; set; }
        public bool Enabled { get; set; }
        public int Priority { get; set; }   
        public List<string> Tags { get; set; }
        public List<Member> MemberData { get; set; } = [];
        
        public class Member
        {
            public string FieldName { get; set; }
            public object? Value { get; set; }
        }
        
        public static BindingFlags FieldSearchFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static;

        public List<SolidReference> Children { get; set; } = [];

        public SolidReference(Component __component) {
            AssemblyName = __component.GetType().Assembly.GetName().Name!;
            ComponentTypeName = __component.GetType().FullName!;
            
            SceneName = __component.Scene!.Name;
            ComponentName = __component.Name;
            
            Enabled = __component.Enabled;
            Priority = __component.Priority;

            Tags = __component.Tags.ToList();

            foreach (Component child in __component) {
                Children.Add(new SolidReference(child));
            }
            
            foreach (FieldInfo field in __component.GetType().GetFields(FieldSearchFlags)) {
                //todo: yuck
                MemberData.Add(new Member{FieldName = field.Name, Value = field.GetValue(__component)});
            }
        }

        [JsonConstructor]
        public SolidReference(string AssemblyName, string ComponentTypeName, string SceneName, string ComponentName, bool Enabled, int Priority, List<string> Tags, List<Member> MemberData) {
            this.AssemblyName = AssemblyName;
            this.ComponentTypeName = ComponentTypeName;
            this.SceneName = SceneName;
            this.ComponentName = ComponentName;
            this.Enabled = Enabled;
            this.Priority = Priority;
            this.Tags = Tags;
            this.MemberData = MemberData;
        }

        public string CreateJson() {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static SolidReference Parse(string __json) {
            return System.Text.Json.JsonSerializer.Deserialize<SolidReference>(__json)!;
        }

        public void Build(ComponentDocker? __parent = null) {
            //todo: invalid json exceptions
            
            Assembly? componentAssembly = Context.LoadedProgram!.Assemblies.First(x => x.GetName().Name == AssemblyName);

            if (componentAssembly == null) {
                //todo: add a component map error system and detect assembly/component name refactors
                Debug.Error("Osmium cannot find a Components assembly! Has its name changed?", ["Assembly Name"], [AssemblyName]);
                return ;
            }
            
            
            //todo: many exceptions
            Type? componentType = componentAssembly.GetType(ComponentTypeName);
            if (componentType == null) {
                Debug.Error("Osmium cannot find a Component's types! Has its name changed or has it moved assemblies?", ["Component Type"], [ComponentTypeName]);
                return ;
            }
            

            Component newComponent;
            try {
                Component? instantiatedComponent = Activator.CreateInstance(componentType) as Component;

                //todo: clean up here  and move out of try catch cuz it breaks if debug is set to throw exceptions
                if (instantiatedComponent == null) {
                    Debug.Error("Osmium failed to create an instance of a Component!");
                    return ;
                }
                
                newComponent = instantiatedComponent;
            } catch(Exception e) {
                Debug.Error("Reinstantiating a referenced Component has failed!", ["Error"], [e.Message]);
                return ;
            }
            
            
            //todo: warning for having to add a scene
            (__parent ?? Osmium.GetScene(SceneName) ?? Osmium.AddScene(SceneName)!).Add(newComponent);
            

            foreach (Member member in MemberData) {
                
                //todo: maybe cache fields and types to speed up reloading?
                FieldInfo? field = componentType.GetField(member.FieldName, FieldSearchFlags);

                if (field == null) {
                    Debug.Error("Osmium cannot find a field belonging to a Component!", ["Field Name"], [member.FieldName]);
                    return ;
                }
                
                field.SetValue(newComponent, member.Value);
            }
            
            newComponent.Name = ComponentName;
            newComponent.Enabled = Enabled;
            newComponent.Priority = Priority;
            //todo: figure out why this is surpressive?

            foreach (string tag in Tags) {
                newComponent.AddTag(tag);
            }

            foreach (SolidReference child in Children) {
                child.Build(newComponent);
            }
        }
    }
}