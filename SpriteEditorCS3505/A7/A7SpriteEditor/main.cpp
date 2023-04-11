#include "mainwindow.h"
#include "canvasmodel.h"
#include <iostream>
#include <QColor>
#include <QApplication>

using namespace std;

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    CanvasModel model;
    MainWindow w(model);
    w.show();

    return a.exec();
}
