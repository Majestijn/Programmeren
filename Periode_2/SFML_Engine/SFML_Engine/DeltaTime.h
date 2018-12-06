#pragma once
class DeltaTime
{
private:
	static float x;
public:
	static float getDeltaTime();
	static float setDeltaTime();

	__event void OnTestEvent();

	void TestFunction();
};


