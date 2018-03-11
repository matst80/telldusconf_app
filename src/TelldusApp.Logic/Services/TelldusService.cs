using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelldusApp.Logic.Rest;
using TelldusApp.Logic.Rest.Models;
using TinyCacheLib;

namespace telldusapp.Services
{
    public class TelldusService
    {
        ITelldusdeviceAPI _api;

        public TelldusService()
        {
            _api = new TelldusdeviceAPI(new Uri("http://localhost:5000"));
        }

        public async Task<IList<Device>> GetDevicesAsync()
        {
            return await TinyCache.RunAsync<IList<Device>>("all_devices", async () => { return await _api.GetAllDevicesAsync(); });
        }

        public async Task<Device> GetDevice(int id)
        {
            var all = TinyCache.GetFromStorage<IList<Device>>("all_devices");
            if (all != null)
            {
                var cacheItem = all.FirstOrDefault(d => d.Id == id);
                if (cacheItem != null)
                    return cacheItem;
            }
            return await _api.GetDeviceByIdAsync(id);
        }
    }
}
