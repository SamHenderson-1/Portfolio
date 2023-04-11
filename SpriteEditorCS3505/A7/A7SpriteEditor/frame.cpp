/*
 * Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
 * CS 3505
 * A7: Qt Sprite Editor
 * Reviewed by: Krish Mahtani
 */
#include <QJsonObject>
#include <QJsonArray>
#include "frame.h"
#include <QColor>
#include <utility>
#include <map>
using namespace std;

// Source: https://stackoverflow.com/questions/1112531/what-is-the-best-way-to-use-two-keys-with-a-stdmap

///@brief setting the frame size
///@param int size - size to be set to
Frame::Frame(int size) {
    this->size = size;
}

///@brief getting certain pixel in the frame from the position provided
///@param int x - x position of the pixel, int y - y position of the pixel
///@return QColor - the color of the pixel
QColor Frame::getPixel(int x, int y) {
    if(pixels.count(make_pair(x, y)) == 0)
        return QColor(255, 255, 255, 255);

    return pixels[make_pair(x, y)];
}

///@brief setting certain pixel in the frame from the position and color provided
///@param int x - x position of the pixel, int y - y position of the pixel, QColor color - the color to set the pixel to
void Frame::setPixel(int x, int y, QColor color) {
    pixels[make_pair(x,y)] = color;
}

///@brief get the size of the frame
///@return int - size of the frame
int Frame::getSize() {
    return size;
}

///@brief to resize the frame
///@param int inputSize - new size for the frame
void Frame::resize(int inputSize) {
    if (inputSize < size) {
        for (int x = inputSize; x < size; x++) {
            for (int y = 0; y < size; y++) {
                if(pixels.count(make_pair(x, y)) != 0) {
                    pixels.erase(make_pair(x, y));
                }
            }
        }
        for (int x = 0; x < inputSize; x++) {
            for (int y = inputSize; y < size; y++) {
                if(pixels.count(make_pair(x, y)) != 0) {
                    pixels.erase(make_pair(x, y));
                }
            }
        }
    }
    size = inputSize;
}

///@brief write the current frame attributs to json
///@param QJsonObject &json - json to be written to, int frameIndex - frame id
// https://stackoverflow.com/questions/1814189/how-to-change-string-into-qstring
void Frame::write(QJsonObject &json, int frameIndex) {
    QString frameName = QString::fromStdString("frame" + to_string(frameIndex));
    json["name"] = frameName;
    QJsonArray totalPixels;
    for (int x = 0; x < size; x++) {
        for (int y = 0; y < size; y++) {
            QColor color = getPixel(x, y);
            QString position = QString::fromStdString(to_string(x) + ", " + to_string(y));
            QJsonArray rgba = {position, qRed(color.rgba()), qGreen(color.rgba()), qBlue(color.rgba()), qAlpha(color.rgba())};
            totalPixels.append(rgba);
        }
    }
    json["pixels"] = totalPixels;
}
