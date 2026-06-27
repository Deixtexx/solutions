using task04;
namespace task04tests
{
    public class SpaceshipTests
    {
        [Fact]
        public void Cruiser_ShouldHaveCorrectStats()
        {
            ISpaceship cruiser = new Cruiser(0.0, 0.0);
            Assert.Equal(50, cruiser.Speed);
            Assert.Equal(100, cruiser.FirePower);
            Assert.Equal(200, cruiser.HP);
        }

        [Fact]
        public void Fighter_ShouldHaveCorrectStats()
        {
            ISpaceship fighter = new Fighter(0.0, 0.0);
            Assert.Equal(100, fighter.Speed);
            Assert.Equal(40, fighter.FirePower);
            Assert.Equal(100, fighter.HP);
        }

        [Fact]
        public void Fighter_ShouldBeFasterThanCruiser()
        {
            var fighter = new Fighter(0.0, 0.0);
            var cruiser = new Cruiser(0.0, 0.0);
            Assert.True(fighter.Speed > cruiser.Speed);
        }

        [Fact]
        public void Cruiser_ShouldHaveMoreFirePower()
        {
            var fighter = new Fighter(0.0, 0.0);
            var cruiser = new Cruiser(0.0, 0.0);
            Assert.True(cruiser.FirePower > fighter.FirePower);
        }

        //для истребителя проверять не нужно, логика движения идентичная
        [Fact]
        public void Cruiser_MovesCorrectly()
        {
            var cruiser = new Cruiser(0.0, 0.0);

            cruiser.Rotate(90);
            cruiser.MoveForward();
            Assert.True(Math.Round(cruiser.PositionX, 1) == 0.0); //в связи с использованием чисел с плавающей точкой
            Assert.True(Math.Round(cruiser.PositionY, 1) == 50.0);
        }

        [Fact]
        public void Cruiser_FiresCorrectly()
        {
            var cruiser = new Cruiser(0.0, 0.0);
            var fighter = new Fighter(5.0, 5.0);
            cruiser.Fire(fighter);
            Assert.Equal(0, fighter.HP);
        }
    }
}
