﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using P04WeatherForecastAPI.Client.Models;
using P04WeatherForecastAPI.Client.Services.WeatherServices;
using P06Shop.Shared.MessageBox;
using P06Shop.Shared.Services.ProductService;
using P06Shop.Shared.Shop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace P04WeatherForecastAPI.Client.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly ProductDetailsView _productDetailsView;
        private readonly IMessageDialogService _messageDialogService;
        public ObservableCollection<Product> Products { get; set; }



        [ObservableProperty]
        private Product selectedProduct;



        public ProductsViewModel(IProductService productService, ProductDetailsView productDetailsView, IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _productDetailsView = productDetailsView;
            _productService = productService;
            Products = new ObservableCollection<Product>();
        }

        public async Task GetProducts()
        {
            Products.Clear();
            var productsResult = await _productService.GetProductsAsync();
            if (productsResult.Success)
            {
                foreach (var p in productsResult.Data)
                {
                    Products.Add(p);
                }
            }
        }

        public async Task CreateProduct()
        {
            var newProduct = new Product()
            {
                Title = selectedProduct.Title,
                Description = selectedProduct.Description,
                Barcode = selectedProduct.Barcode,
                Price = selectedProduct.Price,
                ReleaseDate = selectedProduct.ReleaseDate,
            };

            var result =  await _productService.CreateProductAsync(newProduct);
            if (result.Success)
                await GetProducts();
            else
                _messageDialogService.ShowMessage(result.Message);  
        }

        public async Task UpdateProduct()
        {
            var productToUpdate = new Product()
            {
                Id = selectedProduct.Id,
                Title = selectedProduct.Title,
                Description = selectedProduct.Description,
                Barcode = selectedProduct.Barcode,
                Price = selectedProduct.Price,
                ReleaseDate = selectedProduct.ReleaseDate,
            };

            await _productService.UpdateProductAsync(productToUpdate);
            GetProducts();
        }

        public async Task DeleteProduct()
        {
            await _productService.DeleteProductAsync(selectedProduct.Id);
            await GetProducts();
        }

        [RelayCommand]
        public async Task ShowDetails(Product product)
        {
            _productDetailsView.Show();
            _productDetailsView.DataContext = this;
            //selectedProduct = product;
            //OnPropertyChanged("SelectedProduct");
            SelectedProduct = product;
        }


        [RelayCommand]
        public async Task Save()
        {
            if (selectedProduct.Id == 0)
            {
                CreateProduct();
            }
            else
            {
                UpdateProduct();
            }

        }

        [RelayCommand]
        public async Task Delete()
        {
            DeleteProduct();
        }

        [RelayCommand]
        public async Task New()
        {
            _productDetailsView.Show();
            _productDetailsView.DataContext = this;
            //selectedProduct = new Product();
            //OnPropertyChanged("SelectedProduct");
            SelectedProduct = new Product(); 
        }


    }
}
