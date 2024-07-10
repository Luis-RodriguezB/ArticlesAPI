namespace ArticlesAPI.HandleErrors;
public class ForbiddenException(string message) : Exception(message) { }
