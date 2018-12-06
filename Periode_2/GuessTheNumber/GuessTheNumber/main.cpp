#include <iostream>
#include <cstdlib>
#include <time.h>

class Player {
private:
	int x;
	int y;
};

int main()
{
	int number = rand() % 100;
	int numberToGuess = 0;

	do
	{
		std::cin >> numberToGuess;

		if (numberToGuess < number)
			std::cout << "Higer\n";
		else
			std::cout << "Lower\n";


	} while (numberToGuess != number);

	std::cout << "You guessed the right number!";

	system("pause");

	return EXIT_SUCCESS;
}