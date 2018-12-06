#pragma once

namespace CoreModule {
	class CustomTime
	{
		public:
			static float deltaTime;

			static void SetDeltaTime();
			static float GetDeltaTime();

			static void update();
	};
}

