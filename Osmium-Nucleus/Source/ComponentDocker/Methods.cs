using System.Collections;
using System.Reflection;

namespace OsmiumNucleus;



public abstract partial class Entity
{
    

    
    
    
    /// <summary> Tests to see if every single <see cref="GameObject"/> is a direct child of the <see cref="Entity"/>. </summary>
    /// <param name="Components">The components that are being tested</param>
    /// <returns> True if every <see cref="GameObject"/> in <see cref="Components"/> belongs to the Parent, otherwise false. </returns>
    public bool Contains(params IEnumerable<GameObject> Components) => Components.All(x => _components.Contains(x));

    /// <summary> Tests if the <see cref="Entity"/> contains a components of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of the <see cref="GameObject"/> you want to check for </typeparam>
    /// <returns> True if the Parent has at least one child of type <see cref="TComponent"/>, otherwise false. </returns>
    public bool Contains<TComponent>() where TComponent : GameObject, new() => Get<TComponent>() != null;

    /// <summary> Tests to see if the <see cref="Entity"/> has a child with all the given tags at the very least.</summary>
    /// <param name="Tags"> The list of tags to find on a <see cref="GameObject"/> </param>
    /// <returns> True if there is at last one <see cref="GameObject"/> with at least all the given tags, otherwise false. </returns>
    public bool Contains(params IEnumerable<string> Tags) => GetAll(Tags).Any();

    /// <summary> Tests to see if the <see cref="Entity"/> has a child with at least all the given tags, and of type <see cref="TComponent"/>. </summary>
    /// <param name="Tags"> The list of tags to find on a GameObject </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to search for </typeparam>
    /// <returns> True if there is at least one <see cref="GameObject"/> of type <see cref="TComponent"/> and with all tags in <see cref="Tags"/> </returns>
    public bool Contains<TComponent>(params IEnumerable<string> Tags) where TComponent : GameObject, new() => GetAll<TComponent>(Tags).Any();
    


    /// <summary> Finds the very first <see cref="GameObject"/> owned by the <see cref="Entity"/> that belongs to the <see cref="Entity"/>. </summary>
    /// <returns> The very first <see cref="GameObject"/> owned by the <see cref="Entity"/> found in the <see cref="Entity"/>, null if there is none. </returns>
    public GameObject? Get() => _components.FirstOrDefault();

    /// <summary> Finds the very first <see cref="GameObject"/> owned by the <see cref="Entity"/> that is of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The true of <see cref="GameObject"/> to search for </typeparam>
    /// <returns> The very first <see cref="GameObject"/> owned by the <see cref="Entity"/> of type <see cref="TComponent"/>, null if there is none. </returns>
    public TComponent? Get<TComponent>() where TComponent : GameObject, new() => GetAll<TComponent>().FirstOrDefault();

    /// <summary> Finds the very first <see cref="GameObject"/> owned by the <see cref="Entity"/> that has all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> The tags to search for </param>
    /// <returns> The first <see cref="GameObject"/> owned by the <see cref="Entity"/> that has all the tags present in <see cref="Tags"/>, null if there is none. </returns>
    public GameObject? Get(params IEnumerable<string> Tags) => GetAll(Tags).FirstOrDefault();
    
    /// <summary> Finds the first <see cref="GameObject"/> owned by the <see cref="Entity"/> that is of type <see cref="TComponent"/> and has all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> The tags to search for </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to search for </typeparam>
    /// <returns> The first <see cref="GameObject"/> owned by the <see cref="Entity"/> of type <see cref="TComponent"/> and has all the tags present in <see cref="Tags"/>, null if there is none. </returns>
    public TComponent? Get<TComponent>(params IEnumerable<string> Tags) where TComponent : GameObject, new() => GetAll<TComponent>(Tags).FirstOrDefault();
    
    
    
    /// <summary> Finds all <see cref="GameObject">Components</see> that belong to the <see cref="Entity"/>. </summary>
    /// <returns> Returns a <see cref="List{T}"/> of all <see cref="GameObject">Components</see> that belongs to the <see cref="Entity"/>. </returns>
    public List<GameObject> GetAll() => _components;

    /// <summary> Finds all <see cref="GameObject">Components</see> owned by the <see cref="Entity"/> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent">The type of <see cref="GameObject"/> to search for. </typeparam>
    /// <returns> A <see cref="List{T}"/> of all <see cref="GameObject">Components</see> that belong to the <see cref="Entity"/> and are of type <see cref="TComponent"/>. </returns>
    public List<TComponent> GetAll<TComponent>() where TComponent : GameObject, new() {
        List<TComponent> Result = [];

        foreach (GameObject gameObject in GetAll())
        {
            if (gameObject is TComponent tComponent) {
                Result.Add(tComponent);
            }
        }

        return Result;
    }
    
    /// <summary> Finds all <see cref="GameObject">Components</see> owned by the <see cref="Entity"/> that have all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> All the tags to search for. </param>
    /// <returns> A <see cref="List{T}"/> of all <see cref="GameObject">Components</see> that belong to the <see cref="Entity"/> and have all the tags in <see cref="Tags"/>. </returns>
    public List<GameObject> GetAll(params IEnumerable<string> Tags) {
        if (Tags == null) { Debug.Error("The given Tags cannot be null!"); return []; }
        if (Tags.Contains(null)) { Debug.Error("A given Tag cannot be null!"); return []; }
        if(!Tags.Any()) return GetAll();

        List<GameObject> Result = [];

        foreach (GameObject gameObject in GetAll())
        {
            if (Tags.All(x => gameObject.Tags.Contains(x))) {
                Result.Add(gameObject);
            }
        }

        return Result;
    }

    /// <summary> Finds all the <see cref="GameObject">Components</see> owned by the <see cref="Entity"/> of type <see cref="TComponent"/> that have all the tags in <see cref="Tags"/>. </summary>
    /// <param name="Tags"> All the tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to search for .</typeparam>
    /// <returns> A <see cref="List{T}"/> of all <see cref="GameObject">Components</see> that belong ot the <see cref="Entity"/> of type <see cref="TComponent"/> and have all the tags in <see cref="Tags"/>. </returns>    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public List<TComponent> GetAll<TComponent>(IEnumerable<string> Tags) where TComponent : GameObject, new() {
        if (Tags == null) { Debug.Error("The given Tags cannot be null!"); return []; }
        if (Tags.Contains(null)) { Debug.Error("A given Tag cannot be null!"); return []; }
        if (!Tags.Any()) return GetAll<TComponent>();
        
        List<TComponent> Result = [];

        foreach (GameObject gameObject in GetAll())
        {
            if (gameObject is TComponent tComponent && Tags.All(x => gameObject.Tags.Contains(x))) {
                Result.Add(tComponent);
            }
        }

        return Result;
    }



    /// <summary> Adds every <see cref="GameObject"/> in <see cref="Components"/> as children to the <see cref="Entity"/>. </summary>
    /// <param name="Components"> All the <see cref="GameObject">Components</see> to add. </param>
    public void Add(params GameObject[] Components) {
        foreach (GameObject component in Components) {
            component.Parent = this;
        
            //add to GameObject Parent's lists
            AddComponentToLists(component);
            
            ComponentAdded?.Invoke(this, component);

            if (!component.Destroyed) {
                //create event
                component.TryEvent(Event.Create);
                component.ChainEvent(Event.Create);
            }
        }
    }

    /// <summary> Adds a new <see cref="GameObject"/> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of GameObject to add. </typeparam>
    /// <returns> The new <see cref="GameObject"/> that was created. </returns>
    public TComponent Add<TComponent>() where TComponent : GameObject, new() { TComponent component = new TComponent(); Add(component); return component; }

    /// <summary> Adds a new <see cref="GameObject"/> of type <see cref="TComponent"/> with the name <see cref="Name"/>. </summary>
    /// <param name="Name"> The name of the <see cref="GameObject"/>. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to add. </typeparam>
    /// <returns> The new <see cref="GameObject"/> that was created. </returns>
    public TComponent Add<TComponent>(string Name) where TComponent : GameObject, new() { TComponent component = new TComponent() { Name = Name }; Add(component); return component; }
    
    /// <summary> Adds a new <see cref="GameObject"/> of type <see cref="TComponent"/> and a name of <see cref="Name"/> with the tags in <see cref="Tags"/>. </summary>
    /// <param name="Name"> The name of the <see cref="GameObject"/>. </param>
    /// <param name="Tags"> The tags of the new <see cref="GameObject"/> </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to create. </typeparam>
    /// <returns> The new <see cref="GameObject"/> that was created. </returns>
    public TComponent Add<TComponent>(string Name, params IEnumerable<string> Tags) where TComponent : GameObject, new() { TComponent component = new TComponent() {  Name = Name }; foreach(string tag in Tags) component.Tags.Add(tag); Add(component); return component; }



    /// <summary> Destroys all <see cref="GameObject">Components</see> in <see cref="Components"/>. </summary>
    /// <param name="Components"> The list of <see cref="GameObject">Components</see> to destroy. </param>
    public void Destroy(params IEnumerable<GameObject> Components) {
        foreach (GameObject component in Components) {
            component.TryEvent(Event.Destroy);
            component.ChainEvent(Event.Destroy);

            component.Parent.RemoveComponentFromLists(component);
            component.Parent = null;
            component.Destroyed = true;

            ComponentDestroyed?.Invoke(component.Parent, component);
        }
    }

    /// <summary> Destroys the first found <see cref="GameObject">Components</see> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to destroy. </typeparam>
    public void Destroy<TComponent>() where TComponent : GameObject, new() { TComponent? component = Get<TComponent>(); if (component != null) { Destroy(component); } }

    /// <summary> Destroys the first found <see cref="GameObject">Components</see> that has all the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    public void Destroy(params IEnumerable<string> Tags) { GameObject? component = Get(Tags); if (component != null) { Destroy(component); } }

    /// <summary> Destroys the first found <see cref="GameObject"/> of type <see cref="TComponent"/> with all the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of component to search for. </typeparam>
    public void Destroy<TComponent>(params IEnumerable<string> Tags) where TComponent : GameObject, new() { GameObject? component = Get<TComponent>(Tags); if (component != null) Destroy(component); }


    /// <summary> Destroys all <see cref="GameObject">Components</see> owned by the <see cref="Entity"/>. </summary>
    public void DestroyAll() => Destroy(GetAll());
    
    /// <summary> Destroys all <see cref="GameObject">Components</see> of type <see cref="TComponent"/>. </summary>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to destroy. </typeparam>
    public void DestroyAll<TComponent>() where TComponent : GameObject, new() => Destroy(GetAll<TComponent>());

    /// <summary> Destroys all <see cref="GameObject">Components</see> with all the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    public void DestroyAll(params IEnumerable<string> Tags) => Destroy(GetAll(Tags));

    /// <summary> Destroys all <see cref="GameObject">Components</see> of type <see cref="TComponent"/> with the given tags. </summary>
    /// <param name="Tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to destroy. </typeparam>
    public void DestroyAll<TComponent>(params IEnumerable<string> Tags) where TComponent : GameObject, new() => Destroy(GetAll<TComponent>(Tags));



    /// <summary> Moves all the <see cref="GameObject">Components</see> in <see cref="Components"/> to the given <see cref="Entity"/>. </summary>
    /// <param name="entity"> The docker to move the <see cref="GameObject">Components</see> to. </param>
    /// <param name="Components"> the <see cref="GameObject">Components</see> to move to the other <see cref="Entity"/> </param>
    public void Move(Entity entity, params IEnumerable<GameObject> Components) {
        foreach (GameObject component in Components) {

            if (!component.Destroyed) {
                component.Parent.RemoveComponentFromLists(component);
                entity.AddComponentToLists(component);

                component.Parent = entity;

                ComponentMoved?.Invoke(this, entity, component);
            }
        }
    }

    /// <summary> Moves the first <see cref="GameObject"/> of type <see cref="TComponent"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move the <see cref="GameObject"/> to. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to search for. </typeparam>
    public void Move<TComponent>(Entity entity) where TComponent : GameObject, new() { TComponent? component = Get<TComponent>(); if (component != null) Move(entity, component); }

    /// <summary> Moves the first found <see cref="GameObject"/> with all the given tags, to the given <see cref="Entity"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move the <see cref="GameObject"/> to. </param>
    /// <param name="__tags"> The tags to search for. </param>
    public void Move(Entity entity, params IEnumerable<string> __tags) { GameObject? component = Get(__tags); if(component != null) Move(entity, component); }

    /// <summary> Moves the first found <see cref="GameObject"/> of type <see cref="TComponent"/> to with all the given tags, to the given <see cref="Entity"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move to the <see cref="GameObject"/> to. </param>
    /// <param name="__tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/>. </typeparam>
    public void Move<TComponent>(Entity entity, params IEnumerable<string> __tags) where TComponent : GameObject, new() => Move(entity, Get<TComponent>(__tags));
    
    
    
    /// <summary> Moves all <see cref="GameObject">Components</see> to the given <see cref="Entity"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move all <see cref="GameObject">Components</see> to. </param>
    public void MoveAll(Entity entity) => Move(entity, GetAll());
    
    /// <summary> Moves all <see cref="GameObject">Components</see> of type <see cref="TComponent"/> to the given <see cref="Entity"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move the <see cref="GameObject">Components</see> to. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject">Components</see> to move. </typeparam>
    public void MoveAll<TComponent>(Entity entity) where TComponent : GameObject, new() => Move(entity, GetAll<TComponent>());
    
    /// <summary> Moves all <see cref="GameObject">Components</see> with the given tags, to the <see cref="Entity"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move the <see cref="GameObject">Components</see> to. </param>
    /// <param name="Tags"> The tags to search for. </param>
    public void MoveAll(Entity entity, params IEnumerable<string> Tags) => Move(entity, GetAll(Tags));
    
    /// <summary> Moves all <see cref="GameObject">Components</see> of type <see cref="TComponent"/> to the <see cref="Entity"/>. </summary>
    /// <param name="entity"> The <see cref="Entity"/> to move the <see cref="GameObject">Components</see> to. </param>
    /// <param name="Tags"> The tags to search for. </param>
    /// <typeparam name="TComponent"> The type of <see cref="GameObject"/> to search for. </typeparam>
    public void MoveAll<TComponent>(Entity entity, params IEnumerable<string> Tags) where TComponent : GameObject, new() => Move(entity, GetAll<TComponent>(Tags));
    
    
    
    
    
    public void AddScene(Scene scene) => Osmium.AddScene(scene);
    
    public Scene AddScene(string sceneName) => Osmium.AddScene(sceneName);
    
    public void RemoveScene(Scene scene) => Osmium.RemoveScene(scene);
    
    public void RemoveScene(string sceneName) => Osmium.RemoveScene(sceneName);
    
    public bool ContainsScene(Scene scene) => Osmium.ContainsScene(scene);
    
    public bool ContainsScene(string sceneName) => Osmium.ContainsScene(sceneName);
    
    public void StartCoroutine(IEnumerator<ICoroutineAction> coroutine) => Osmium.StartCoroutine(coroutine);

    public float DeltaTime() => Osmium.DeltaTime;

}