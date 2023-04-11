/*
Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
CS3505
A7: Qt Sprite Editor
Reviewed by: Tracy Garner

This file sets up connections for all signals and slots from the ui
 */
#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "clickablelabel.h"
#include <QLabel>
#include <QString>
#include <QMessageBox>
#include <QInputDialog>
#include <QFileDialog>
#include <iostream>
#include <QDebug>

///@brief   this is the constructor for the main window
///         the user interface is set up and individual components are added
///         the connections are added to make the components work with the model
/// @param CanvasModel& model - a reference to the model
/// @param QWidget *parent - a pointer to the user interface
MainWindow::MainWindow(CanvasModel& model, QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    //the brush is set to disabled since it is currently selected
    ui->brushButton->setDisabled(true);
    string labelText = "Animation FPS: " + std::to_string(ui->fpsSlider->value());
    ui->fpsLabel->setText(QString::fromStdString(labelText));
    model.setFPS(ui->fpsSlider->value());
    //our resources for custom icons for the buttons
    ui->brushButton->setIcon(QIcon(":/icons/paintbrush.png"));
    ui->eraserButton->setIcon(QIcon(":/icons/eraser.png"));
    ui->eyedropperButton->setIcon(QIcon(":/icons/eyedropper.png"));
    ui->onionSkinningButton->setIcon(QIcon(":/icons/onion.png"));

    currFileName = "";
    //the canvas is a label that has a qpixmap
    ClickableLabel *canvasLabel = new ClickableLabel(this);
    canvasLabel->setText("canvas");
    canvasLabel->setGeometry(190,140,300,300);

    //frameSizeSpinBox valueChanged signal is connected to the setFrameSize method in CanvasModel
    connect(ui -> frameSizeSpinBox,
            &QSpinBox::valueChanged,
            &model,
            &CanvasModel::setFrameSize);

    //addFrameButton clicked signal is connected to the newFrame method in CanvasModel
    connect(ui -> addFrameButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::newFrame);
    //playButton toggled signal is connected to the toggleAnimation method in CanvasModel
    connect(ui->playButton,
            &QPushButton::toggled,
            &model,
            &CanvasModel::toggleAnimation);
    //nextPreviewFrame from CanvasModel is connected to the previewLabel
    //in the ui through the setPixmap method in QLabel
    connect(&model,
            &CanvasModel::nextPreviewFrame,
            ui->previewLabel,
            &QLabel::setPixmap);
    //emitNewPixmap from CanvasModel is connected to the canvasLabel
    //which is initialized above, it inherits setPixmap from QLabel
    connect(&model,
            &CanvasModel::emitNewPixmap,
            canvasLabel,
            &ClickableLabel::setPixmap);
    //emitNewPixmap from CanvasModel with the previous frame
    //in the ui through the setPixmap method in QLabel
    connect(&model,
            &CanvasModel::emitNewPreviewPixmap,
            ui -> lastFrameLabel,
            &QLabel::setPixmap);
    //onionSkinningButton clicked signal is connected to the toggleOnionSkin method from CanvasModel
    connect(ui -> onionSkinningButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::togglePreviousFrame);
    //mouseMoved signal from ClickableLabel is connected to the mouseMoved method from CanvasModel
    connect(canvasLabel,
            &ClickableLabel::mouseMoved,
            &model,
            &CanvasModel::mouseMoved);
    //coordinates method from CanvasModel is connected to the pixelLocationLabel
    //in the ui through the setText method from QLabel
    connect(&model,
            &CanvasModel::coordinates,
            ui->pixelLocationLabel,
            &QLabel::setText);
    //fpsSlider valueChanged signal is connected to the setFPS method in CanvasModel
    connect(ui->fpsSlider,
            &QSlider::valueChanged,
            &model,
            &CanvasModel::setFPS);
    //changePenColorButton clicked signal is connected to the changePenColorButtonPressed method in CanvasModel
    connect(ui->changePenColorButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::changePenColorButtonPressed);
    //brushButton clicked signal is connected to the brushSelected method in CanvasModel
    connect(ui->brushButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::brushSelected);
    //eyedropperButton clicked signal is connected to the eyeDropperSelected method in CanvasModel
    connect(ui->eyedropperButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::eyeDropperSelected);
    //eraserButton clicked signal is connected to the eraserSelected method in CanvasModel
    connect(ui->eraserButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::eraserSelected);
    //goForwardButton clicked signal is connected to the goForwardOneFrame method in CanvasModel
    connect(ui->goForwardButton,
                &QPushButton::clicked,
                &model,
                &CanvasModel::goForwardOneFrame);
    //goBackwardButton clicked signal is connected to the goBackwardOneFrame method in CanvasModel
    connect(ui -> goBackwardButton,
            &QPushButton::clicked,
            &model,
            &CanvasModel::goBackwardOneFrame);
    //the MainWindow save signal (related to Save and SaveAs in menu bar)
    //is connected to the save method in CanvasModel
    connect(this,
            &MainWindow::save,
            &model,
            &CanvasModel::save);
    //the MainWindow load signal (related to Open in menu bar)
    //is connected to the load method in CanvasModel
    connect(this,
            &MainWindow::load,
            &model,
            &CanvasModel::loadSpriteFromString);
    //the MainWindow newFile signal (related to New in the menu bar)
    //is connected to the createNewSprite method in the CanvasModel
    connect(this,
            &MainWindow::newFile,
            &model,
            &CanvasModel::createNewSprite);
    //the changeSizeInView signal from the CanvasModel
    //is connected the frameSizeSpinBox setValue method in QSpinBox
    connect(&model,
            &CanvasModel::changeSizeInView,
            ui->frameSizeSpinBox,
            &QSpinBox::setValue);
    //the showFrameNumber signal from the CanvasModel
    //is connected to the MainWindow showFrameNumber function
    connect(&model,
            &CanvasModel::showFrameNumber,
            this,
            &MainWindow::showFrameNumber);
    //refreshes the screen
    model.resetDisplay();
}
///@brief deletes the ui pointer
MainWindow::~MainWindow()
{
    delete ui;
}

///@brief   when the value of the frames per second slider changes
///         the label for Animation FPS changes
/// @param int value
void MainWindow::on_fpsSlider_valueChanged(int value)
{
    string labelText = "Animation FPS: " + std::to_string(value);
    ui->fpsLabel->setText(QString::fromStdString(labelText));
}

///@brief   when the playButton is toggled on (true)
///         the label for the playButton changes to "Stop Animation"
///         when the playButton is toggled off (false)
///         the label for the playButtonchanges to "Play Animation"
///@param bool checked
void MainWindow::on_playButton_toggled(bool checked)
{
    if (checked) {
        ui->playButton->setText(QString::fromStdString("Stop Animation"));
    }
    else {
        ui->playButton->setText(QString::fromStdString("Play Animation"));
    }
}
///@brief   when the brushButton is toggled on (true)
///         the eraserButton and eyedropperButton are toggled to false
/// @param bool checked
void MainWindow::on_brushButton_toggled(bool checked){
    toggleButton(ui->brushButton, checked);
    ui->eraserButton->toggled(false);
    ui->eyedropperButton->toggled(false);
}
///@brief   when the eraserButton is toggled on (true)
///         the brushButton and eyedropperButton are toggled to false
/// @param bool checked
void MainWindow::on_eraserButton_toggled(bool checked){
    toggleButton(ui->eraserButton, checked);
    ui->brushButton->toggled(false);
    ui->eyedropperButton->toggled(false);
}
///@brief   when the eyedropperButton is toggled on (true)
///         the brushButton and eraserButton are toggled to false
/// @param bool checked
void MainWindow::on_eyedropperButton_toggled(bool checked)
{
    toggleButton(ui->eyedropperButton, checked);
    ui->brushButton->toggled(false);
    ui->eraserButton->toggled(false);
}
///@brief   when the onionSkinningButton is toggned
///         the value of the button is checked with the toggleButton
///         on is true, off is false
/// @param bool checked
void MainWindow::on_onionSkinningButton_toggled(bool checked){
    toggleButton(ui->onionSkinningButton, checked);
}
///@brief   when this method is called, it changes the color of the button clicked
///         true (on) changes the color to a shade of blue
///         false (off) changes the color to a shade of grey
/// @param QPushButton *pressedButton
/// @param bool checked
void MainWindow::toggleButton(QPushButton *pressedButton, bool checked){
    if(checked) {
        pressedButton->setStyleSheet("QPushButton {background-color:rgb(173,216,230);}");
    }
    else{
        pressedButton->setStyleSheet("QPushButton {background-color:rgb(220,220,220);}");
    }
}
///@brief   when this method is called the current frame number
///         is sent to the label to show the user the current frame
///         the textfor frameNumLabel is set to "Frame " currentFrame
/// @param currentFrame
void MainWindow::showFrameNumber(int currentFrame) {
    string displayText = "Frame " + std::to_string(currentFrame);
    ui->frameNumLabel->setText(QString::fromStdString(displayText));
}
///@brief   when the brushButton is clicked the brushButton is disabled
///         the eraserButton and eyedropperButton are enabled
void MainWindow::on_brushButton_clicked()
{
    ui->brushButton->setDisabled(true);
    ui->eraserButton->setDisabled(false);
    ui->eyedropperButton->setDisabled(false);
}

///@brief   when the eraserButton is clicked the eraserButton is disabled
///         the brushButton and eyedropperButton are enabled
void MainWindow::on_eraserButton_clicked()
{
    ui->brushButton->setDisabled(false);
    ui->eraserButton->setDisabled(true);
    ui->eyedropperButton->setDisabled(false);
}

///@brief   when the eyedropperButton is clicked the eyedropperButton is disabled
///         the brushButton and the eraserButton are enabled
void MainWindow::on_eyedropperButton_clicked()
{
    ui->brushButton->setDisabled(false);
    ui->eraserButton->setDisabled(false);
    ui->eyedropperButton->setDisabled(true);
}

///@brief   when the Open menu option is clicked the saveMessage QMessageBox is opened
///         a QMessageBox is used to prompt the user to Save, Discard, or Cancel the action
///         if the Save button is clicked the onActionSaved method is triggered
///         then the openFilePrompt method is triggered
///         if the Discard button is clicked the openFilePrompt method is triggered
///         if the Cancel button is selected no action are taken
void MainWindow::on_actionOpen_triggered()
{
    QMessageBox saveMessage;
    saveMessage.setText("Warning!");
    saveMessage.setInformativeText("Do you want to save your changes?");
    saveMessage.setStandardButtons(QMessageBox::Save | QMessageBox::Discard | QMessageBox::Cancel);
    int clicked = saveMessage.exec();
    if(clicked == QMessageBox::Save) {
        on_actionSave_triggered();
        openFilePrompt();
    }
    else if(clicked == QMessageBox::Discard) {
        openFilePrompt();
    }
}

///@brief   when the openFilePrompt method is triggered a QFileDialog window is opened
///         the prompt retrieves the FileName for the file to be opened
///         the prompt filters files, sprite files are .ssp file extention
///         if the filename is not empty it is saved as currentFileName and sent to the load signal
void MainWindow::openFilePrompt() {
    QString qFileName = QFileDialog::getOpenFileName(this, tr("Open File"), "./", tr("Sprite files (*.ssp)"));
    if(!qFileName.isNull()) {
        string fileName = qFileName.toStdString();
        currFileName = fileName;
        emit load(fileName);
    }
}

///@brief   when the New menu option is clicked the saveMessage QMessageBox is opened
///         a QMessageBox is used to prompt the user to Save, Discard, or Cancel the action
///         if the Save button is clicked the onActionSaved method is triggered
///         then the newFile signal is emitted
///         if the Discard button is clicked the newFile signal is emitted
///         if the Cancel button is selected no action are taken
void MainWindow::on_actionNew_triggered()
{
    QMessageBox saveMessage;
    saveMessage.setText("Warning!");
    saveMessage.setInformativeText("Do you want to save your changes?");
    saveMessage.setStandardButtons(QMessageBox::Save | QMessageBox::Discard | QMessageBox::Cancel);
    int clicked = saveMessage.exec();
    if(clicked == QMessageBox::Save) {
        on_actionSave_triggered();
        emit newFile(16);
    }
    else if(clicked == QMessageBox::Discard) {
        emit newFile(32);
    }
}

///@brief   when the Save As menu option is clicked, a QInputDialog box prompts the user for a filename
///         it is converted to a string, set to the variable currFileName and is emited from the save signal
void MainWindow::on_actionSave_As_triggered()
{
    QString qFileName = QInputDialog::getText(this, tr("Save File"), tr("FileName: "));
    string fileName = qFileName.toStdString();
    currFileName = fileName;
    emit save(fileName);
}

///@brief   when the Save menu option is clicked, it is checked if the file was already created
///         if the filename is size 0, then the Save As function is triggered
///         if not the save signal is sent out
void MainWindow::on_actionSave_triggered()
{
    if(currFileName.length() == 0) {
        on_actionSave_As_triggered();
    }
    else {
        emit save(currFileName);
    }
}

///@brief   when the Help menu option is clicked a helpMessage is sent out
///         it explains to the user what the interface elements do
void MainWindow::on_actionHelp_triggered()
{
     QMessageBox helpMessage;
     helpMessage.setText("What to do:");
     helpMessage.setInformativeText("Adjust FPS, edit frames and draw to make your own animation. \nUse a variety of different colors from the pallet. \nUse Onion skinning to display previous frame to better your animation. \nBest of Luck.");
     helpMessage.exec();
}
