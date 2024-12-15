using UngureanuIoanAlexandruLab7.Models;
namespace UngureanuIoanAlexandruLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
        this.BindingContext)
        {
            BindingContext = new Product()
        });
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");
        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem is Product selectedProduct)
        {
            var shopList = (ShopList)this.BindingContext;

            var listProduct = await App.Database.GetListProductAsync(shopList.ID, selectedProduct.ID);

            if (listProduct != null)
            {
                await App.Database.DeleteListProductAsync(listProduct);

                listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);

                await DisplayAlert("Success", $"Product '{selectedProduct.Description}' has been removed.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "The selected product is not linked to this shopping list.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "No item selected.", "OK");
        }
    }
} 

