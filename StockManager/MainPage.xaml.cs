using StockManager.Pages;
using StockManager.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockManager
{
    public partial class MainPage : ContentPage
    {
        private readonly IStockService _stockService;
        private ShoppingPage _shoppingPage;
        public ICommand StorageCommand { get; set; }
        public ICommand ShoppingCommand { get; set; }
        public ICommand RecipesCommand { get; set; }

        public MainPage(IStockService stockService)
        {
            InitializeComponent();

            _stockService = stockService;
            StorageCommand = new Command(OnStorageButtonClicked);
            ShoppingCommand = new Command(OnShoppingButtonClicked);
            RecipesCommand = new Command(OnRecipesButtonClicked);
            _shoppingPage = new ShoppingPage(stockService);
            BindingContext = this;
        }

        private async void OnStorageButtonClicked()
        {
            await Navigation.PushAsync(new StoragePage(_stockService));
        }
        private async void OnShoppingButtonClicked()
        {
            await Navigation.PushAsync(_shoppingPage);
        }
        private async void OnRecipesButtonClicked()
        {
            await Navigation.PushAsync(new RecipesPage(_stockService));
        }
    }

}
