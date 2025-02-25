using MVCSample.Infrastructure;

namespace MVCSample.Infrastructure
{
    public class Module<ModelT, ViewT, ControllerT>
    {
        private readonly ModelT _model;
        private readonly ViewT _view;
        private readonly ControllerT _controller;

        private readonly Context _currentContext;

        public Module(Context currentContext, ViewT view)
        {
            _currentContext = currentContext;
            _view = view;

            _model = _currentContext.ResolveDeep<ModelT>();
            _controller = _currentContext.ResolveDeep<ControllerT>();


        }
    }
}
