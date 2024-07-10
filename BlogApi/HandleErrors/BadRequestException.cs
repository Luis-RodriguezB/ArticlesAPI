namespace ArticlesAPI.HandleErrors;
public class BadRequestException(string message) : Exception(message) { }
