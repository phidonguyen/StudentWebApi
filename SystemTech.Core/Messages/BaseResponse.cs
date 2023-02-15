namespace SystemTech.Core.Messages
{
    public abstract class BaseResponse<TResult, TField> : BaseRequest<TField> where TResult : class, new() where TField: class
    {
        private int _total;
        public int Total => _total;
        
        private TResult _result;
        public TResult Result => _result;

        protected BaseResponse(BaseRequest<TField> tRequest) : base(tRequest.Fields)
        {
            _result = new TResult();
        }

        public void SetTotal(int value) => _total = value;
        public void SetResult(TResult result) => _result = result;
    }
}