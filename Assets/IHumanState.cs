public interface IHumanState
{ 
    IHumanState DoState(Human human);
    void UseCurrentTarget(Human human);
}
