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

        constructor($resource: ng.resource.IResourceService) {
            this.AuctionResource = $resource('/api/auctions/:id');
        }
    }

    angular.module('AuctionApp').service('AuctionService', AuctionService);


    }
