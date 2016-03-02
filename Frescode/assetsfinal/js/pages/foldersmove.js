//ToDo: refactor
;(function(){

	function onlyUnique(value, index, self) { 
	    return self.indexOf(value) === index;
	}

	var rootViewModel=new Object();

	var FoldersList = function(arr,viewModel){
				var self = this;
				self.level=ko.observable(0);
				self.path=ko.observable("");
				self.folders=ko.observableArray();
				self.files = ko.observableArray();

				self.drawingsStructure = new Array(); 

				$.each(arr,function(key,val){
						var splittedFolder = val.folder.split('/');
						self.drawingsStructure.push({folder: val.folder,splittedFolder:splittedFolder,name:val.name, id:val.id});		

				});
				var folders=new Array();
				var files=new Array();
				$.each(self.drawingsStructure,function(key,val){
							if (val.splittedFolder[self.level()]!=="") {
								folders.push(val.splittedFolder[self.level()]);
							}
							else if (val.splittedFolder[self.level()]===""){
								files.push({name:val.name,id:val.id});	
							}
						});
				var foldersunique=folders.filter(onlyUnique);
				var filesunique=files.filter(onlyUnique);

				$.each(foldersunique,function(key,val){
					self.folders.push(val);
				});

				$.each(filesunique,function(key,val){

					self.files.push({name:val.name,id:val.id});
				});

				rootViewModel=viewModel;

			};

	function folderIn(elem){
		rootViewModel.mainViewModel().foldersList().path(rootViewModel.mainViewModel().foldersList().path()+elem+'/');
		rootViewModel.mainViewModel().foldersList().level(rootViewModel.mainViewModel().foldersList().level()+1);
		foldersMove(rootViewModel.mainViewModel().foldersList().level());
		
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
	    
	}

	function folderOut(elem){
		var arr = rootViewModel.mainViewModel().foldersList().path().split('/');
		arr.splice(arr.length-2,2);
		var a=arr.join("/");
		if (a!=="") {
			a=a+'/';
		} 
		rootViewModel.mainViewModel().foldersList().path(a);
		rootViewModel.mainViewModel().foldersList().level(rootViewModel.mainViewModel().foldersList().level()-1);
		foldersMove(rootViewModel.mainViewModel().foldersList().level());

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
	}

	function foldersMove(lev){
		var goodArr=new Array();			
		
		$.each(rootViewModel.mainViewModel().foldersList().drawingsStructure,function(key,val){
			if (val.folder.startsWith(rootViewModel.mainViewModel().foldersList().path())){
				//console.log(val.folder);
				goodArr.push(rootViewModel.mainViewModel().foldersList().drawingsStructure[key])
			}
		});

		rootViewModel.mainViewModel().foldersList().folders.removeAll();
		rootViewModel.mainViewModel().foldersList().files.removeAll();
		
		var folders=new Array();
		var files=new Array();
		
		$.each(goodArr,function(key,val){
			if (val.splittedFolder.length>lev && val.splittedFolder[lev]!=="") {
				folders.push(val.splittedFolder[lev]);
			}
			else if (val.splittedFolder[lev]===""){
				files.push({name:val.name,id:val.id});	
			}
		});

		var foldersunique=folders.filter(onlyUnique);
		var filesunique=files.filter(onlyUnique);

		$.each(foldersunique,function(key,val){
			rootViewModel.mainViewModel().foldersList().folders.push(val);
		});

		$.each(filesunique,function(key,val){
			rootViewModel.mainViewModel().foldersList().files.push({name:val.name,id:val.id});
		});



	}

	window.FoldersList = FoldersList;
	window.folderIn = folderIn;
	window.folderOut = folderOut;

}());