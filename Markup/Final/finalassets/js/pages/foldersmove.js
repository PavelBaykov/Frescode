//ToDo: refactor
;(function(){

	var FolderViewModel = function(){
		var self=this;
		self.name=ko.observable();
	};

	var FileViewModel = function(){
		var self=this;
		self.id = ko.observable();
		self.name=ko.observable();

		self.onClickCommand = function(){
			window.location.href="InspectionDrawing/"+self.id;
		}
	};

	function onlyUnique(value, index, self) { 
	    return self.indexOf(value) === index;
	}

	function initFunction(arr,drawingsStructure){
		$.each(arr,function(key,val){
			var splittedFolder = val.folder.split('/');
			drawingsStructure.push({folder: val.folder,splittedFolder:splittedFolder,name:val.name, id:val.id});		
		});
	}

	function fillStructure(drawingsStructure, level, foldersObserve, filesObserve){
		var folders=new Array();
		var files=new Array();
		$.each(drawingsStructure,function(key,val){
			if (val.splittedFolder[(level())]!=="") {
				folders.push(val.splittedFolder[level()]);
			}
			else if (val.splittedFolder[level()]==="" && val.name!=""){
				files.push({name:val.name,id:val.id});	
			}
		});
		var foldersunique=folders.filter(onlyUnique);
		var filesunique=files.filter(onlyUnique);

		$.each(foldersunique,function(key,val){
			var folderItem=new FolderViewModel();
			folderItem.name=val;
			foldersObserve.push(val);
		});

		$.each(filesunique,function(key,val){
			var fileItem=new FileViewModel();
			fileItem.id=val.id;
			fileItem.name=val.name;
			filesObserve.push(fileItem);
		});
	}

	function addAnimation(){
	// Add animate options
			var animateMap = {},
			animateOptions = {
				easing: 'easeInOut',
				duration: 250
			};
			animateMap["backgroundColorAlpha"] = 0.8;
	        animateMap["backgroundColor"] = "#F5F5F5";

	        // Add animation class to panel element
	        $(".custom-box")
	        .velocity(animateMap, animateOptions)
	        .velocity("reverse", {
	        	delay: 100,
	        	complete: function() {
	        		$(this).removeAttr('style');
	        	}
	        });
	};

	function foldersMove(lev,drawingsStructure,path,foldersObserve,filesObserve){
		var goodArr=new Array();			
		
		$.each(drawingsStructure,function(key,val){
			if (val.folder.startsWith(path)){
				goodArr.push(drawingsStructure[key])
			}
		});

		foldersObserve.removeAll();
		filesObserve.removeAll();
		
		fillStructure(goodArr, lev, foldersObserve, filesObserve);
	}

	var FoldersList = function(){
			
			var self = this;
			self.level=ko.observable(0);
			self.path="";
			self.folders=ko.observableArray();
			self.files = ko.observableArray();

			self.drawingsStructure = new Array(); 

			self.init = function(arr){
				initFunction(arr,self.drawingsStructure);
				fillStructure(self.drawingsStructure,self.level,self.folders,self.files );	
			};	



			self.folderOut = function(elem){
				var arr = self.path.split('/');
				arr.splice(arr.length-2,2);
				var cuttedPath=arr.join("/");
				if (cuttedPath!=="") {
					cuttedPath=cuttedPath+'/';
				};
				self.path=cuttedPath;
				self.level(self.level()-1);
				foldersMove(self.level,self.drawingsStructure,self.path,self.folders,self.files);

				addAnimation();
			};

			self.folderIn = function(elem){
				self.path=self.path+elem+'/';
				self.level(self.level()+1);
				foldersMove(self.level,self.drawingsStructure,self.path,self.folders,self.files);
		
		        addAnimation();
			};			
	}

	window.FoldersList = FoldersList;
}());