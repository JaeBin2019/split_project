#include <iostream>
#include <algorithm>

using namespace std;

int main()
{

    float web, flg, thick_w;
    cout << "height(web) : ";
    cin >> web;
    cout << "width(flg) : ";
    cin >> flg;
    cout << "thick_w : ";
    cin >> thick_w;

    float width = flg - thick_w + 50;

    float D, R;
    if (web <= 150)
    {
        R = 0;
    }
    else if (web <= 300)
    {
        R = 50;
    }
    else if (web <= 450)
    {
        R = 75;
    }
    else
    {
        R = 100;
    }

    if (web <= 250)
    {
        D = 50;
    }
    else
    {
        D = 70;
    }
    float height = web - D;

    printf("plate => 높이 : %.1f, 가로 : %.1f, R : %.1f\n", height, width, R);
}
