namespace ArticlesAPI.HandleErrors;
public class NotFoundException(string message) : Exception(message) { }
