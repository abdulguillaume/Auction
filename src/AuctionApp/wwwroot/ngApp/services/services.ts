namespace AuctionApp.Services {

    export class AuctionService {
        private AuctionResource;

        public listAuctions() {
            //debugger;
            return this.AuctionResource.query();
        }

        public getAuction(id) {
            return this.AuctionResource.get({ id: id });
        }

        public save(auction) {
            return this.AuctionResource.save(auction).$promise;
        }

        public deleteAuction(id)
        {
            return this.AuctionResource.delete(id).$promise;
        }

        constructor(public $resource: ng.resource.IResourceService) {
            //debugger;
            this.AuctionResource = $resource('/api/auctions/:id');

        }
    }


    //not used
    //http://stackoverflow.com/questions/41307705/angularjs-pass-data-from-one-view-to-another-with-in-same-controller
    //export function DataService() {

    //    var staticData = {};

    //    function setData(data, key) {

    //        staticData[key] = data;

    //    }



    //    function getData(key) {

    //        return staticData[key];

    //    }

    //}

    angular.module('AuctionApp').service('AuctionService', AuctionService);



    }
