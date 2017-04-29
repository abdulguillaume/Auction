namespace AuctionApp.Services {

    export class AuctionWithBidsService {
        private AuctionWithBidsResource;

        public listAuctions() {
            return this.AuctionWithBidsResource.query();
        }

        public getAuction(id) {
            return this.AuctionWithBidsResource.get({ id: id });
        }

        public save(bid) {
            this.AuctionWithBidsResource.save(bid).$promise;
        }

        constructor($resource: ng.resource.IResourceService) {
            this.AuctionWithBidsResource = $resource('/api/auctions/:id');
        }
    }

    angular.module('AuctionApp').service('AuctionWithBidsService', AuctionWithBidsService);

    //angular.module('AuctionApp').filter('max', function () {
    //    return function (values) { }
    //});

    /*
    export class AuctionService {
        private AuctionResource;

        public listAuctions() {
            return this.AuctionResource.query();
        }

        public getAuction(id) {
            return this.AuctionResource.get({ id: id });
        }

        constructor($resource: ng.resource.IResourceService) {
            this.AuctionResource = $resource('/api/auctions/:id');
        }
    }*/

    }
