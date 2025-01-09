public class SceneName
{
    private readonly string _sceneName;

    private SceneName(string sceneName)
    {
        _sceneName = sceneName;
    }

    public static SceneName Title => new SceneName("Title");
    public static SceneName Menu => new SceneName("Menu");
    public static SceneName Main => new SceneName("Main");
    public static SceneName Result => new SceneName("Result");

    public override string ToString()
    {
        return _sceneName;
    }
}