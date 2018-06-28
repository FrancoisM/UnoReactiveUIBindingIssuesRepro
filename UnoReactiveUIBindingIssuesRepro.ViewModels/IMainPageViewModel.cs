using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace UnoReactiveUIBindingIssuesRepro.ViewModels
{
    public class MainPageViewModel : ReactiveObject, IMainPageViewModel, IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private ReactiveList<IProductCellViewModel> _products;
        private readonly ObservableAsPropertyHelper<bool> _isRefreshing;
        public ReactiveList<IProductCellViewModel> Products
        {
            get => _products;
            private set => this.RaiseAndSetIfChanged(ref _products, value);
        }
        public bool IsRefreshing => _isRefreshing.Value;
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        public MainPageViewModel()
        {
            var products = new ReactiveList<IProductCellViewModel>
            {
                new ProductCellViewModel("Car", "20 000€"),
                new ProductCellViewModel("House", "400 000€")
            };
            RefreshCommand = ReactiveCommand.CreateFromObservable(() => Observable
                    .Timer(TimeSpan.FromSeconds(2))
                    .ObserveOn(scheduler:RxApp.MainThreadScheduler)
                    .Do(_ => Products = products)
                    .Select(_ => Unit.Default))
                .DisposeWith(_disposables);
            _isRefreshing = RefreshCommand.IsExecuting.ToProperty(this, x => x.IsRefreshing);
            RefreshCommand.ThrownExceptions.Subscribe(err => Console.WriteLine(err.ToString()));
            RefreshCommand.Execute(Unit.Default).Subscribe().DisposeWith(_disposables);
        }

        public void Dispose() => _disposables?.Dispose();

        private class ProductCellViewModel : IProductCellViewModel
        {
            public string Product { get; }
            public string Price { get; }

            public ProductCellViewModel(string product, string price)
            {
                Product = product;
                Price = price;
            }
        }
    }

    public interface IMainPageViewModel
    {
        bool IsRefreshing { get; }
        ReactiveList<IProductCellViewModel> Products { get; }
        ReactiveCommand<Unit, Unit> RefreshCommand { get; }
    }

    public interface IProductCellViewModel
    {
        string Product { get; }
        string Price { get; }
    }
}
