/*
Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
CS3505
A7: Qt Sprite Editor
Reviewed by: Tracy Garner

Sets up the methods needed for mainwindow.h
 */
#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QPushButton>
#include "canvasmodel.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

///@brief this class represents the main view the user sees
/// @inherits QMainWindow
class MainWindow : public QMainWindow
{
    //a special QT macro to set up an object
    Q_OBJECT

public:
    //the default constructor
    MainWindow(CanvasModel& model, QWidget *parent = nullptr);
    //destructor
    ~MainWindow();
signals:
    //sent when a file is being saved
    void save(std::string fileName);
    //sent when a file is being loaded
    void load(std::string fileName);
    //sent when a new file is created
    void newFile(int len);

private slots:
    //when the slider for frames per second is changed
    void on_fpsSlider_valueChanged(int value);
    //when the button is pressed for playing the animation
    void on_playButton_toggled(bool checked);
    //when the brush button is clicked on is set to true
    void on_brushButton_toggled(bool checked);
    //when the eraser button is clicked on is set to true
    void on_eraserButton_toggled(bool checked);
    //when the eyedropper button is clicked on is set to true
    void on_eyedropperButton_toggled(bool checked);
    //when the onionSkinning button is clicked on is set to true
    void on_onionSkinningButton_toggled(bool checked);
    //when the brush button is clicked
    void on_brushButton_clicked();
    //when the eraser button is clicked
    void on_eraserButton_clicked();
    //when the eyedropper button is clicked
    void on_eyedropperButton_clicked();
    //when the file menu Open option is clicked
    void on_actionOpen_triggered();
    //when the file menu New option is clicked
    void on_actionNew_triggered();
    //when the file menu Save As option is clicked
    void on_actionSave_As_triggered();
    //when the file menu Save option is clicked
    void on_actionSave_triggered();
    //when the file menu Help option is clicked
    void on_actionHelp_triggered();
    //the label for frame number is changed
    void showFrameNumber(int currentFrame);

private:
    //user interface object
    Ui::MainWindow *ui;
    //filename
    string currFileName;
    //when a button is toggled, this is a helper method to switch other buttons off
    void toggleButton(QPushButton *pressedButton, bool checked);
    //when a file is opened
    void openFilePrompt();
};
#endif // MAINWINDOW_H
