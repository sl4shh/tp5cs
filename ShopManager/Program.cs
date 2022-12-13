using System;
using ShopManager;
using ShopManager.Products;
using ShopManager.Products.Food;
using ShopManager.Products.Wine;

var s = new Shop(1000f,new Product[]{new Moussaka(7),new GodBlood(),new Feta(27)});
ShopConsole.Run(s);