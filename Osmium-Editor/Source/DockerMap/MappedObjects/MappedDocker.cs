using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor;

public abstract class SerializedDocker(ComponentDocker docker)
{
    
    public ComponentDocker docker = docker;
    public List<ComponentMapNode> Children = docker.Children.Select(x => new ComponentMapNode(x)).ToList();
    
    public List<SerializedVariable> SerializedFields = [];
    
    public static implicit operator ComponentDocker(SerializedDocker node) => node.docker;

}