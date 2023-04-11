/*
 * Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
 * CS 3505
 * A7: Qt Sprite Editor
 * Reviewed by: Ben Rose
 *
 * This class stores the pixel and frame data for representing the sprite
 */
#include <QJsonObject>
#include <QJsonArray>
#include <QFile>
#include <QJsonDocument>
#include <fstream>
#include <vector>
#include "sprite.h"
#include "frame.h"
using namespace std;

///
/// @brief Sprite::Sprite creates a new Sprite with dimensions lxl. Pixels are
///         white by default.
/// @param l - height and width, so Sprite is lxl
///
Sprite::Sprite(int l){
    sideLength = l;
}

///
/// @brief Sprite::Sprite creates a Sprite based on the input JSON file. Reads
///         pixel colors, frames, and height and width.
/// @param fileName - includes name and directory of file for reading
///
Sprite::Sprite(string fileName) {
    QFile loadFile(QString::fromStdString(fileName));

   if (!loadFile.open(QIODevice::ReadOnly)) {
       qWarning("Couldn't open load file.");
       return;
   }

   QByteArray saveData = loadFile.readAll();
   QJsonDocument loadDoc(QJsonDocument::fromJson(saveData));

   read(loadDoc.object());
}

///
/// @brief Sprite::GetDimensions returns width and height of the square sprite
/// @return width and height
///
int Sprite::getDimensions() {
    return sideLength;
}

///
/// @brief Sprite::GetFrameCount returns number of frames the sprite has stored
/// @return number of frames
///
int Sprite::getFrameCount() {
    return frames.size();
}

///
/// @brief Sprite::GetPixel finds the color of a certain pixel
/// @param x - horizontal coordinate
/// @param y - vertical coordinate
/// @param frame - index of the frame based on the sprite's frame order
/// @return color (QColor) of the pixel
///
QColor Sprite::getPixel(int x, int y, int frame) {
    return frames[frame].getPixel(x, y);
}

///
/// @brief Sprite::SetPixel changes the color of the indicated pixel
/// @param x - horizontal coordinate
/// @param y - vertical coordinate
/// @param frame - index of the frame to set the pixel
/// @param color (QColor) of the pixel
///
void Sprite::setPixel(int x, int y, int frame, QColor color) {
    frames[frame].setPixel(x, y, color);
}

///
/// @brief Sprite::AddFrame adds a new blank white frame to the Sprite, using
///         the same dimensions
///
void Sprite::addFrame() {
    frames.push_back(Frame(sideLength));
}

///
/// @brief Sprite::RemoveFrame removes a frame at the indicated index
/// @param indexFrame - location of the frame based on the sprite's frame order
///
void Sprite::removeFrame(int indexFrame) {
    frames.erase(frames.begin()+indexFrame);
}

///
/// @brief Sprite::Save writes the Sprite to a file with a .ssp extension,
///         using JSON to save the data. Stores RGBA values for the pixels,
///         height and width, and number of frames.
/// @param fileName - name to give the written file, will be saved in the A7
///         build folder
/// Used source: https://doc.qt.io/qt-6/qtcore-serialization-savegame-example.html
///
void Sprite::save(string fileName) {
    QFile saveFile(QString::fromStdString(fileName + ".ssp"));

    if (!saveFile.open(QIODevice::WriteOnly)) {
        qWarning("Couldn't open save file.");
        return;
    }

    QJsonObject spriteObject;
    write(spriteObject);
    saveFile.write(QJsonDocument(spriteObject).toJson());
    saveFile.close();
}

///
/// @brief Sprite::Write helper method for Sprite::save, writes the information
///         using JSON
/// @param json - QJsonObject representing the file
/// Used source: https://doc.qt.io/qt-6/qtcore-serialization-savegame-example.html
///
void Sprite::write(QJsonObject &json) {
    json["height"] = sideLength;
    json["width"] = sideLength;
    json["numberOfFrames"] = (int) frames.size();

    QJsonArray frameArray;
    for (unsigned int i = 0; i < frames.size(); i++) {
        QJsonObject frameObject;
        frames[i].write(frameObject, i);
        frameArray.append(frameObject);
    }
    json["frames"] = frameArray;
}

///
/// @brief Sprite::Resize changes the dimensions of each frame of the sprite.
///         Resizing happens upward and leftward, so the top left corner
///         remains in place.
/// @param inputSize - new size to set the sprite to
///
void Sprite::resize(int inputSize) {
    for (unsigned int i = 0; i < frames.size(); i++) {
        frames[i].resize(inputSize);
    }
    sideLength = inputSize;
}

///
/// @brief Sprite::Read helper method for building a Sprite out of an input
///         JSON file. Uses the input JSON listing the pixel data, height and
///         width, and number of frames to fill in the member variables of this
///         object.
/// @param json - QJsonObject representing the file
/// Used source: https://doc.qt.io/qt-6/qtcore-serialization-savegame-example.html
///
void Sprite::read(const QJsonObject &json) {
    if(json.contains("height"))
        sideLength = json["height"].toInt();

    if(json.contains("numberOfFrames")) {
        for(int i = 0; i < json["numberOfFrames"].toInt(); i++)
            addFrame();
    }

    if (json.contains("frames") && json["frames"].isArray()) {
        QJsonArray framesArray = json["frames"].toArray();
        for(int i = 0; i < framesArray.size(); i++) {
            QJsonObject frameObject = framesArray[i].toObject();
            QJsonArray pixelsArray = frameObject["pixels"].toArray();
            setPixelsFromJsonArray(pixelsArray, i);
        }
    }
}

///
/// @brief Sprite::SetPixelsFromJsonArray helper for Sprite::read method that
///         takes in a QJsonArray that lists out the pixels and their RGBA values
///         and uses it to set the pixels of this object.
/// @param pixelsArray - QJsonArray that lists out pixels in the format used by
///         the Sprite class
/// @param frameIndex - index of the frame to draw the pixel on
/// Used source: https://doc.qt.io/qt-6/qtcore-serialization-savegame-example.html
///
void Sprite::setPixelsFromJsonArray(QJsonArray pixelsArray, int frameIndex) {
    // sideLength * sideLength = total # pixels
    for(int j = 0; j < (sideLength*sideLength); j++) {
        QJsonArray rgbaArray = pixelsArray[j].toArray();
        std::string coordinates = rgbaArray[0].toString().toStdString();

        int x = stoi(coordinates.substr(0, coordinates.find(",")));
        int y = stoi(coordinates.substr(coordinates.find(",")+2));
        int r = rgbaArray[1].toInt();
        int g = rgbaArray[2].toInt();
        int b = rgbaArray[3].toInt();
        int a = rgbaArray[4].toInt();
        QColor color(r, g, b, a);
        setPixel(x, y, frameIndex, color);
    }
}


