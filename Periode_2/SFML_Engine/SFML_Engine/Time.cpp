#include "Time.h"
#include <SFML/Graphics.hpp>

sf::Clock * clock;

void update() {
	CoreModule::Time::SetDeltaTime(clock->restart().asSeconds());
}

float GetDeltaTime()
{
	return CoreModule::Time::deltaTime;
}
