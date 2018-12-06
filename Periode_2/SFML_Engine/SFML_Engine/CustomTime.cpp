#include "CustomTime.h"
#include <SFML/Graphics.hpp>

namespace CoreModule {

	sf::Clock * clock = new sf::Clock();

	void CustomTime::SetDeltaTime()
	{
		CustomTime::deltaTime = clock->restart().asSeconds();
	}

	float CustomTime::GetDeltaTime()
	{
		return CustomTime::deltaTime;
	}

	void CustomTime::update()
	{
		CustomTime::SetDeltaTime();
	}
}