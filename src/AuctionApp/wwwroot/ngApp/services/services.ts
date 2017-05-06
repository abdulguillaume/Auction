namespace AuctionApp.Services {

    export class AuctionService {
        private AuctionResource;

        public listAuctions() {
            return this.AuctionResource.query();
        }

        public getAuction(id) {
            return this.AuctionResource.get({ id: id });
        }

        public save(auction) {
            return this.AuctionResource.save(auction).$promise;
        }

        /*public upload(formData) {

            let promise = new Promise( (resolve, reject) => {

                let TmpResource = this.$resource('/api/upload/', null, {
                    save: {
                        method: 'POST',
                        transformRequest: formData,
                        
                        headers: { 'Content-Type': undefined, enctype: 'multipart/form-data' }
                    }
                });

                debugger;

                TmpResource.save();


            });

            return promise;

        }*/

        constructor(public $resource: ng.resource.IResourceService) {
            this.AuctionResource = $resource('/api/auctions/:id');

        }
    }

    angular.module('AuctionApp').service('AuctionService', AuctionService);


    }
