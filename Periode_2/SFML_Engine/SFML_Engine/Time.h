#pragma once

namespace CoreModule {
	class Time
	{
		public:
			//variables
			static float deltaTime;

			//functions
			static float GetDeltaTime();
			static void SetDeltaTime(float value);

			void update();
	};
}



