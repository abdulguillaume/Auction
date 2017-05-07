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

    
    

    export class AuctionsController {

        public auctions; 

        public auctionToCreate;
        public auctionToEdit;
        public auction_tmp = { id: 0, name: null, description: null, minimumBid: 0, numberOfBids: 0, deletedImgs : [] };

        public files;

        public emsg;//error message to show on the screen

        constructor(private AuctionService: AuctionApp.Services.AuctionService, private $state: ng.ui.IStateService, private $uibModal: angular.ui.bootstrap.IModalService, private Upload: angular.angularFileUpload.IUploadService, private $stateParams: ng.ui.IStateParamsService)
        {
            if (this.$stateParams) {
                this.auctionToEdit = this.AuctionService.getAuction(this.$stateParams.id);
            } 

        }

        public cachePropsInDummyObject(source) {
            this.auction_tmp.id = source.id;
            this.auction_tmp.name = source.name;
            this.auction_tmp.description = source.description;
            this.auction_tmp.minimumBid = source.minimumBid;
            this.auction_tmp.numberOfBids = source.numberOfBids;
        }

        public getData()
        {
            this.auctions = this.AuctionService.listAuctions();
        }

        //public getAuction(id)
        //{
        //    let tmp = this.auctions.filter(a => a.id == id);

        //    if (tmp.length == 0)
        //        this.auctionToEdit = null;
        //    else
        //        this.auctionToEdit = tmp[0];

        //        //no need for a round trip to the service, just use the one in cache
        //        //this.AuctionService.getAuction(id);
        //}

        public modelStateOk(auction)
        {
            if (auction && auction.name &&
                auction.description &&
                auction.minimumBid &&
                auction.numberOfBids )
                return true;
            else
                return false;
        }

        //to refresh the auctions list
        //public refresh(that) {
        //    that.auctions = that.AuctionService.listAuctions();
        //}

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
                    //that.refresh(that);
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

        public loadInBrowser(files)
        {
            this.files = files;
        }


        public removeImg(id) {

            this.auctionToEdit.images = this.auctionToEdit.images.filter(x => x.id != id);
            console.log("remove img id: " + id);
            this.auction_tmp.deletedImgs.push(id);

        }

        public uploadFormData(val)
        {
            //val: 1 => create a new auction
            //val: 2 => update an auction

          /*http://public.jsweet.org/apidocs/snapshots/org/jsweet/candies/ng-file-upload/11.1.1-SNAPSHOT/def/ng_file_upload/ng/angularfileupload/IFileUploadConfigFile.html
            */
            let auction = val == 1 ? this.auctionToCreate : this.auctionToEdit;
            this.cachePropsInDummyObject(auction);
            debugger;
            this.Upload.upload({
                method: 'post',
                data: {
                    //transformRequest: fd,
                    files: this.files,
                    fields: this.auction_tmp, //auctionToCreate is already a json object
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

        }

    }


    function FuncUploadFormData(Upload: angular.angularFileUpload.IUploadService, files, auction) {

        //http://public.jsweet.org/apidocs/snapshots/org/jsweet/candies/ng-file-upload/11.1.1-SNAPSHOT/def/ng_file_upload/ng/angularfileupload/IFileUploadConfigFile.html

        return Upload.upload({
            method: 'post',
            data: {
                //transformRequest: fd,
                files: files,
                fields: auction, //auctionToCreate is already a json object
                headers: { 'Content-Type': undefined, enctype: 'multipart/form-data' }
            },
            url: '/api/upload'
        });
          
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
                },

                getAll: {
                    method: 'GET'
                }
            });

            resource.save(auction)
                .$promise
                .then(
                    response =>
                    {
                        resource.getAll();
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


        constructor(private id: number, private item: string, private username: string, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, private $resource: ng.resource.IResourceService) {

        }
    }

    angular.module('AuctionApp').controller('DialogController', DialogController);


}
