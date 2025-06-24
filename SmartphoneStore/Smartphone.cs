using System;

public class Smartphone
{
    private float _screenSize;
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int Storage { get; set; } 
    public int RAM { get; set; } 
    public string Processor { get; set; }
    public float ScreenSize
    {
        get => _screenSize;
        set => _screenSize = (float)Math.Round(value, 1); // Округляем до 1 знака после запятой
    }
    public int BatteryCapacity { get; set; } 
    public int Quantity { get; set; } 
}