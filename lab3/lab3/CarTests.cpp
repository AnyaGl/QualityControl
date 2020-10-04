#define CATCH_CONFIG_MAIN
#include "Car.h"
#include "catch2/catch.hpp"

TEST_CASE("Engine must be off by default")
{
	CCar car;
	CHECK(!car.IsEngineOn());
}

TEST_CASE("Speed must be in range of current gear")
{
	CCar car;
	car.TurnOnEngine();
	car.SetGear(1);

	CHECK(car.SetSpeed(30));
	CHECK(car.GetSpeed() == 30);

	CHECK(car.SetSpeed(0));
	CHECK(car.GetSpeed() == 0);

	CHECK(car.SetSpeed(15));
	CHECK(car.GetSpeed() == 15);

	CHECK(!car.SetSpeed(-1));

	CHECK(!car.SetSpeed(31));
}

TEST_CASE("On neutral gear speed must only decrease")
{
	CCar car;
	car.TurnOnEngine();

	car.SetGear(1);
	car.SetSpeed(10);
	car.SetGear(0);
	CHECK(car.SetSpeed(5));
	CHECK(car.GetSpeed() == 5);
	CHECK(!car.SetSpeed(10));
}

TEST_CASE("Direction must change according to speed and gear")
{
	SECTION("If gear >= 1 and speed != 0 then direction must be 'Forward'")
	{
		CCar car;
		car.TurnOnEngine();
		car.SetGear(1);

		CHECK(car.GetDirection() == Direction::Stand);
		car.SetSpeed(30);
		CHECK(car.GetDirection() == Direction::Forward);
	}

	SECTION("If gear == -1 and speed != 0 then direction must be 'Back'")
	{
		CCar car;
		car.TurnOnEngine();
		car.SetGear(-1);

		CHECK(car.GetDirection() == Direction::Stand);
		car.SetSpeed(20);
		CHECK(car.GetDirection() == Direction::Back);
	}
}

TEST_CASE("You can change the gear if current speed in speed range of new gear and direction allow")
{
	CCar car;
	car.TurnOnEngine();
	car.SetGear(1);

	car.SetSpeed(10);

	CHECK(!car.SetGear(2)); // speed doesn't in speed range

	CHECK(car.SetGear(1));
	CHECK(car.GetGear() == 1);

	CHECK(!car.SetGear(-1)); // direction doesn't allow
}

TEST_CASE("Engine must be turned off at zero speed in neutral gear")
{
	CCar car;
	car.TurnOnEngine();

	car.SetGear(1);
	car.SetSpeed(10);
	CHECK(!car.TurnOffEngine());

	car.SetGear(0);
	CHECK(!car.TurnOffEngine());

	car.SetSpeed(0);
	CHECK(car.TurnOffEngine());
}

TEST_CASE("If engine is off, only neutral gear can be")
{
	CCar car;

	CHECK(!car.SetGear(1));
	CHECK(!car.SetGear(-1));
	CHECK(car.SetGear(0));
}