/*
 * Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
 * CS 3505
 * A7: Qt Sprite Editor
 * Reviewed by: Krish Mahtani
 */
#ifndef FRAME_H
#define FRAME_H

#include <QJsonObject>
#include <map>
#include <utility>
#include <QColor>
using std::map;
using std::pair;

///@brief The Frame class holds all the attributes for the current frame
class Frame
{
public:
    //frame id
    Frame(int);
    //get certain pixel in the frame
    QColor getPixel(int x, int y);
    //set certain pixel in the frame
    void setPixel(int x, int y, QColor color);
    //get size of the frame
    int getSize();
    //resize the frame
    void resize(int);
    //helper method to save the project (write frame attributes)
    void write(QJsonObject &json, int frameIndex);

private:
    //hold pixels
    map<pair<int, int>, QColor> pixels;
    //holds the size of the frame
    int size;
};

#endif // FRAME_H
