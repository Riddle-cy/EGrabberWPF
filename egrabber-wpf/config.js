// Euresys Grabber Configuration Script
//memento.notice('Euresys Configuration Script');
//var g = grabbers[0];
// TODO: please add your code here to configure g


function config(grabber){
	grabber.RemotePort.set("AcquisitionFrameRate","500");
}

config(grabber[0]);
