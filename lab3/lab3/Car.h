#pragma once

enum class Direction
{
	Forward,
	Back,
	Stand
};

class CCar
{
public:
	bool TurnOnEngine();
	bool TurnOffEngine();
	bool IsEngineOn() const;

	bool SetGear(int gear);
	int GetGear() const;

	bool SetSpeed(int speed);
	int GetSpeed() const;

	Direction GetDirection() const;
private:
	bool m_isEngineOn = false;
	Direction m_direction = Direction::Stand;
	int m_speed = 0;
	int m_gear = 0;
};

