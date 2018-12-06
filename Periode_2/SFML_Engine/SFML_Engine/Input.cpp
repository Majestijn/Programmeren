#include "Input.h"
#include <SFML/Graphics.hpp>

namespace CoreModule {
	std::map<sf::Keyboard::Key, bool> Input::keyMap;

	bool Input::GetKeyDown(sf::Keyboard::Key key)
{
	if (sf::Keyboard::isKeyPressed(key))
	{
		if (!keyMap[key])
		{
			keyMap[key] = true;
			return true;
		}
		else
		{
			return false;
		}

	}
	else
	{
		keyMap[key] = false;
		return false;
	}
}

	bool Input::GetKey(sf::Keyboard::Key key)
{
	return sf::Keyboard::isKeyPressed(key);
}

	bool Input::GetKeyUp(sf::Keyboard::Key key)
{
	if (!sf::Keyboard::isKeyPressed(key))
	{
		if (keyMap[key])
		{
			keyMap[key] = false;
			return true;
		}
	}
	else
	{
		keyMap[key] = true;
		return false;
	}
	return false;
}
}


