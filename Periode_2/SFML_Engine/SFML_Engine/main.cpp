#include <iostream>
#include <SFML/Graphics.hpp>
#include "CoreModule.h"

using namespace CoreModule;

int main()
{
	sf::RenderWindow window(sf::VideoMode(200, 200), "TestGame");

	while (window.isOpen())
	{
		CustomTime::update();

		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
		}

		std::cout << CustomTime::GetDeltaTime() << std::endl;

		if (Input::GetKeyDown(sf::Keyboard::Space))
		{
			std::cout << "Do something" << std::endl;
		}

		if (Input::GetKeyDown(sf::Keyboard::Escape))
		{
			window.close();
		}

		window.clear();
		window.display();
	}

	return EXIT_SUCCESS;
}