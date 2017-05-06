namespace AuctionApp.Controllers {

    export class HomeController {
        public message = 'Hello from the home page!';
    }

    export class SecretController {
        public secrets;

        constructor($http: ng.IHttpService) {
            $http.get('/api/secrets').then((results) => {
                this.secrets = results.data;
            });
        }
    }


    export class  AuctionsController {
        public auctions; 
        public auctionToCreate;

        public files;

        public emsg;//error message to show on the screen

        constructor(private AuctionService: AuctionApp.Services.AuctionService, private $state: ng.ui.IStateService, private $uibModal: angular.ui.bootstrap.IModalService, private Upload: angular.angularFileUpload.IUploadService)
        {
            //this.auctions = AuctionService.listAuctions();     
        }

        public getData()
        {
            this.auctions = this.AuctionService.listAuctions();
        }

        public modelStateOk()
        {
            if (this.auctionToCreate && this.auctionToCreate.name &&
                this.auctionToCreate.description &&
                this.auctionToCreate.minimumBid &&
                this.auctionToCreate.numberOfBids )
                return true;
            else
                return false;
        }

        //to refresh the auctions list
        public refresh(that) {
            that.auctions = that.AuctionService.listAuctions();
        }

        public showModal(id: number, item: string, username: string) {

            let that = this;

            this.$uibModal.open({
                templateUrl: '/ngApp/views/auctionDialog.html',
                controller: 'DialogController',
                controllerAs: 'modal',
                resolve: {
                    id: () => id,
                    item: () => item,
                    username: () => username
                },
                size: 'sm'
            })
                .result
                .then(function () {
                    that.refresh(that);
                });

        }

        public getFilesCount()
        {
            if (this.files == null) {
                return 0;
            }
            else {
                this.emsg = '';
                return this.files.length;
            }
        }

        //public addAuction() {
        //    this.AuctionService.save(this.auctionToCreate)
        //        .then(
        //        () => {
        //            this.auctionToCreate = null;
        //            this.$state.go('home');
        //        }
        //        );
        //}


        public upload(files)
        {
            this.files = null;

            for (let file of files)
            {
                console.log(file);
            }

            this.files = files;
        }

        public uploadFormData() {

            /*let fd = new FormData();

            fd.append('files[]', this.files);

            fd.append('name', this.auctionToCreate.name);

            fd.append("description", this.auctionToCreate.description);

            fd.append("minimumBid", this.auctionToCreate.minimumBid);

            fd.append("numberOfBids", this.auctionToCreate.numberOfBids);
            */

            //http://public.jsweet.org/apidocs/snapshots/org/jsweet/candies/ng-file-upload/11.1.1-SNAPSHOT/def/ng_file_upload/ng/angularfileupload/IFileUploadConfigFile.html


            debugger;

            this.Upload.upload({
                method: 'post',
                data: {
                    //transformRequest: fd,
                    files: this.files,
                    fields: this.auctionToCreate, //auctionToCreate is already a json object
                    headers: { 'Content-Type': undefined, enctype: 'multipart/form-data' }
                },
                url: '/api/upload'
            })
                .then((result) => {
                    console.log('File upload complete.');
                    this.emsg = '';
                    this.$state.go('home');
            }).catch((err) => {
                console.error('Could not upload file!');
                this.emsg = err.data;
                this.files = null;
            });


            //debugger;
        }





    }

    export class AddController {
        public message = 'Hello from the about page!';
    }


    export class AboutController {
        public message = 'Hello from the about page!';
    }

    
    class DialogController {

        public bid;

        public error = false;

        public emsg;

        public ok() {
           
            let auction = {
                id: this.id,
                bids: [{ id: 0, bidAmount: this.bid, customer: this.username }]
            };

            let resource: any = this.$resource('/api/auctions/:id', null, {
                save: {
                    method: 'POST'
                }
            });

            resource.save(auction)
                .$promise
                .then(
                    response =>
                    {
                        this.$uibModalInstance.close();
                        this.error = false;
                    }
                )
                .catch(
                    err =>
                    {
                        this.error = true;
                        this.emsg = err.data;
                    }
                );

                
            }


        constructor(private id: number, private item: string, private username: string, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, private $resource: ng.resource.IResourceService ){}
    }

    angular.module('AuctionApp').controller('DialogController', DialogController);


}
