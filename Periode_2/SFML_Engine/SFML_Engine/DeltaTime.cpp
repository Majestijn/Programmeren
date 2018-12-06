#include "DeltaTime.h"
#include <SFML/Graphics.hpp>

float DeltaTime::x = 0;

sf::Clock * clock = new sf::Clock();

float DeltaTime::getDeltaTime()
{
	return x;
}

float DeltaTime::setDeltaTime()
{
	x = clock->restart().asSeconds();
	OnTestEvent += 
}

void DeltaTime::OnTestEvent()
{

}
