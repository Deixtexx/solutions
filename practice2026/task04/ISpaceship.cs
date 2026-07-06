namespace task04
{
    public interface ISpaceship
    {
        void MoveForward();      // Движение вперед
        void Rotate(int angle);  // Поворот на угол (градусы)
        void Fire(ISpaceship target); // Выстрел ракетой по врагу
        int Speed { get; }       // Скорость корабля
        int FirePower { get; }   // Мощность выстрела
        int HP { get; set; }     // Показатель здоровья
    }
}
