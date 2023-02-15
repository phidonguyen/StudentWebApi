namespace SystemTech.Core.Messages
{
    public abstract class BaseRequest<TField> where TField: class
    {
        private List<Exception> _errors; 
        public List<Exception> ErrorMessages => _errors;

        private TField _fields;
        public TField Fields => _fields;
        
        protected BaseRequest(TField tField)
        {
            _errors = new List<Exception>();
            _fields = tField;
        }

        
        public void AddException(Exception exception)
        {
            _errors.Add(exception);
        }
        
        public void AddException(List<Exception> exceptions)
        {
            _errors.AddRange(exceptions);
        }

        public bool HasErrors() => _errors.Any();
        
        public void SetFields(TField fields)
        {
            _fields = fields;
        }
    }
}