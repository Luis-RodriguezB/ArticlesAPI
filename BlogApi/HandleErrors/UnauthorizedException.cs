namespace ArticlesAPI.HandleErrors;
public class UnauthorizedException(string message) : Exception(message) { }
