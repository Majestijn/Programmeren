#pragma once
#include <SFML/Graphics.hpp>

namespace CoreModule {
	class Input
	{
	public:
		static std::map<sf::Keyboard::Key, bool> keyMap;

		static bool GetKeyDown(sf::Keyboard::Key key);
		static bool GetKey(sf::Keyboard::Key key);
		static	bool GetKeyUp(sf::Keyboard::Key key);
	};
}


